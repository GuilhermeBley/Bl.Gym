import { createContext } from "react";

export const UserContext = createContext({});

export default function UserContextProvider({children} : any, user: any){
    return (
        <UserContext.Provider value={user}>
            {children}
        </UserContext.Provider>
    );
}