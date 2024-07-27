import { createContext, useState } from "react";
import jwtDecoder from 'jwt-decode';

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

    isInRole(roles: string | string[]): boolean {
        if (typeof roles === 'string') {
            return this.roles.includes(roles);
        }
        return roles.some(role => this.roles.includes(role));
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

export const UserContext = createContext<UserContextProps>({
    user: unauthorizedUser,
    login: (token) => { },
    logout: () => { }
});

export default function UserContextProvider({children} : any){

    const [user, setUser] = useState(unauthorizedUser)

    // TODO: check user cache

    const login = (jwtToken: string): void => {
        try{
            const decoded: any = jwtDecoder.jwtDecode(jwtToken);

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
                decoded.authorized,
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
    
    return (
        <UserContext.Provider value={{ user, login, logout }}>
            {children}
        </UserContext.Provider>
    );
}