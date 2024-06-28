import { createContext } from "react";

interface UserContextProps{
    id: string;
    name: string;
    email: string;
    roles: string[];
    authorized: boolean;
    isInRole(roles: string | string[]) : boolean;
    setUserByJwtToken(jwtToken: string) : void;
}

const unauthorizedUser: UserContextProps = {
    id: "",
    name: "",
    email: "",
    roles: [],
    authorized: false,
    isInRole: () => false,
    setUserByJwtToken: () => { }
};

export const UserContext = createContext<UserContextProps>(unauthorizedUser);

export default function UserContextProvider({children} : any, user: UserContextProps){

    return (
        <UserContext.Provider value={user}>
            {children}
        </UserContext.Provider>
    );
}