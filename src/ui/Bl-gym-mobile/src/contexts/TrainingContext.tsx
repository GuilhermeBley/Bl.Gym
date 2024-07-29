import { createContext, useState } from "react";

interface TrainingContextModel{
    trainingId: string,
    trainingName: string,
    trainingDescription: string
}

interface TrainingContextProps{
    training: TrainingContextModel | undefined,
    setTrainingContext: (training: TrainingContextModel | undefined) => void
}

export const TrainingContext = createContext<TrainingContextProps>({
    training: undefined,
    setTrainingContext: (_) => { }
});

export default function UserContextProvider({children} : any){

    const [training, setTrainingContext]
        = useState<TrainingContextModel | undefined>(undefined)
    
    return (
        <TrainingContext.Provider value={{ training, setTrainingContext }}>
            {children}
        </TrainingContext.Provider>
    );
}