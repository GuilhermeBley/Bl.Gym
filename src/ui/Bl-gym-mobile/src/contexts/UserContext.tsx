import { createContext } from "react";
import jwtDecoder from 'jwt-decode';

class UserContextProps{
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

    setUserByJwtToken(jwtToken: string): void {
        try{
            const decoded: any = jwtDecoder.jwtDecode(jwtToken);

            if (!decoded ||
                parseInt(decoded.id) < 1)
                this.setAsUnauthorized();
                return;
    
            // Update the user properties based on the decoded token
            this.id = decoded.id;
            this.name = decoded.name;
            this.email = decoded.email;
            this.roles = decoded.roles;
            this.authorized = decoded.authorized;
        }
        catch{
            console.error("Failed to parse login.")
        }
    }

    setAsUnauthorized()
    {
        this.id = "";
        this.name = "";
        this.email = "";
        this.roles = [];
        this.authorized = false;
    }
}

const unauthorizedUser = new UserContextProps(
    "", "", "", [], false
);

export const UserContext = createContext<UserContextProps>(unauthorizedUser);

export default function UserContextProvider({children} : any, user: UserContextProps){

    return (
        <UserContext.Provider value={user}>
            {children}
        </UserContext.Provider>
    );
}