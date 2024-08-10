import { createContext, useEffect, useState } from "react";
import * as jwtDecode from 'jwt-decode';
import { getAuthorization } from "../services/AuthStorageService";
import { ActivityIndicator, View } from "react-native";

class UserContextModel{
    id: string;
    name: string;
    email: string;
    roles: string[];
    authorized: boolean;

    constructor(id: string, name: string, email: string, roles: string[], authorized: boolean) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.roles = roles;
        this.authorized = authorized;
    }

    isInRole(rolesToCheck: string | string[]): boolean {
        if (typeof rolesToCheck === 'string') {
            return this.roles.includes(rolesToCheck);
        }
        return rolesToCheck.every(role => this.roles.includes(role));
    }
}

interface UserContextProps{
    user: UserContextModel,
    login: (jwtToken: string) => void,
    logout: () => void
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

export const UserContext = createContext<UserContextProps>({
    user: unauthorizedUser,
    login: (_) => { },
    logout: () => { }
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
    
                if (token !== undefined && token !== null && token !== '') {
                    login(token)
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

    const login = (jwtToken: string): void => {
        try{
            const decoded: any = jwtDecode.jwtDecode(jwtToken);

            if (!decoded || parseInt(decoded.id) < 1){
                logout();
                return;
            }
            
            // Update the user properties based on the decoded token
            setUser(new UserContextModel(
                decoded.id,
                decoded.name,
                decoded.email,
                decoded.roles,
                true,
            ))
        }
        catch(error) {
            logout();
            console.error("Failed to parse login.", error)
        }
    }

    const logout = () => {
        setUser(unauthorizedUser)
    }
    
    return(
        <UserContext.Provider value={{ user, login, logout }}>
            {pageStatus.authorizationLoaded ? children : PageNotProperlyLoadedComponent()}
        </UserContext.Provider>
    );
}