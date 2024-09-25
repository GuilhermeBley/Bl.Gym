import { createContext, useEffect, useState } from "react";
import * as jwtDecode from 'jwt-decode';
import { getAuthorization, getRefreshToken, storeAuthorization, storeRefreshToken } from "../services/AuthStorageService";
import { ActivityIndicator, View } from "react-native";
import axios from "../api/GymApi";

class UserContextModel{
    id: string;
    name: string;
    email: string;
    refreshToken: string | undefined;
    roles: string[];
    authorized: boolean;
    dueDate: Date | undefined;

    constructor(
        id: string,
        name: string, 
        email: string, 
        roles: string[], 
        authorized: boolean, 
        dueDate: Date | undefined = undefined) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.roles = roles;
        this.authorized = authorized;
        this.dueDate = dueDate;
    }

    isInRole(rolesToCheck: string | string[]): boolean {
//
        if (!this.authorized)
            return false;

        if (!Array.isArray(this.roles))
            return false;

        if (typeof rolesToCheck === 'string') {
            return this.roles.includes(rolesToCheck);
        }
        return rolesToCheck.every(role => this.roles.includes(role));
    }

    isAuthorized(){
        return this.id?.length > 10;
    }

    isExpirated(){
        const currentTime = new Date;

        if (this.dueDate === undefined)
            return true

        var expirated = this.dueDate < currentTime;
        console.debug(`[` + currentTime +`] User token will expire at ` + this.dueDate)
        return expirated;
    }
}

interface UserContextProps{
    user: UserContextModel,
    login: (jwtToken: string, refreshToken: string) => Promise<void>,
    logout: () => Promise<void>
}

const unauthorizedUser = new UserContextModel(
    "", "", "", [], false
);

const PageNotProperlyLoadedComponent = () => {
    return (
        <View>
            <ActivityIndicator/>
        </View>
    );
}

const getRolesFromDecoded = (decoded: any) => {
    if (Array.isArray(decoded.role)){
        return decoded.role;
    }
    
    if (typeof decoded.role === "string"){
        return [decoded.role];
    }

    return [];
}

export const UserContext = createContext<UserContextProps>({
    user: unauthorizedUser,
    login: (token, _) => Promise.resolve(),
    logout: () => Promise.resolve()
});

export default function UserContextProvider({children} : any){

    const [user, setUser] = useState(unauthorizedUser)
    const [pageStatus, setPageStatus] = useState({
        authorizationLoaded: false,
    });

    useEffect(() => {

        const fetchData = async () => {

            try{
                console.debug('getAuthorization started.')
                let token = await getAuthorization();
                let refreshToken = await getRefreshToken();
    
                if (token !== undefined && token !== null && token !== '') {
                    token = token.replace("Bearer ", "")
                    await login(token, refreshToken ?? "")
                }
            }
            finally{
                console.debug('getAuthorization executed.')

                setPageStatus(previous => ({
                    ...previous,
                    authorizationLoaded: true  
                }))

                console.debug('previous.authorizationLoaded = ', pageStatus.authorizationLoaded)
            }
        };

        fetchData();

        return () => { /*nothing to dispose*/ }
    }, [])

    const login = async (jwtToken: string, refreshToken: string): Promise<void> => {
        try{
            const decoded: any = jwtDecode.jwtDecode(jwtToken);

            if (!decoded || typeof decoded.nameidentifier !== "string"){
                await logout();
                return;
            }

            var userToAuthorize =
                new UserContextModel(
                    decoded.nameidentifier,
                    (decoded.firstname + ' ' + decoded.lastname),
                    decoded.emailaddress,
                    getRolesFromDecoded(decoded),
                    true,
                    typeof decoded.exp === "number" ? new Date(decoded.exp * 1000) : undefined
                );
            
            userToAuthorize.refreshToken = refreshToken;
            
            // Update the user properties based on the decoded token
            setUser(userToAuthorize)

            let bearerToken = 'Bearer ' + jwtToken;
            axios.defaults.headers.common['Authorization'] =  bearerToken;
            await storeAuthorization(bearerToken)
            await storeRefreshToken(refreshToken)
        }
        catch(error) {
            await logout();
            console.error("Failed to parse login.", error)
        }
    }

    const logout = async () => {
        await storeAuthorization('')
        await storeRefreshToken('')
        setUser(unauthorizedUser)
    }
    
    return(
        <UserContext.Provider value={{ user, login, logout }}>
            {pageStatus.authorizationLoaded ? children : PageNotProperlyLoadedComponent()}
        </UserContext.Provider>
    );
}