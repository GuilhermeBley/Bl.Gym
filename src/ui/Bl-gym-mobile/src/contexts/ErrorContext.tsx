import React, { createContext, useContext, useState, ReactNode } from "react";

interface Error {
  type: "Warning" | "Error" | "Critical";
  message: string;
}

interface ErrorContextType {
  errors: Error[];
  currentErrorIndex: number;
  addError: (error: Error | string | Error[] | string[]) => void;
  closeCurrentError: () => void;
  clearErrors: () => void;
}

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);

export const ErrorProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [errors, setErrors] = useState<Error[]>([]);
  const [currentErrorIndex, setCurrentErrorIndex] = useState<number>(0);

  const addError = (error: Error | string | Error[] | string[]) => {

    if (Array.isArray(error)) {

      if (error.every((item) => typeof item === "string"))
        error = error.map(e => { return { type: "Error", message: e } as Error })

      setErrors(error);

      return;
    }

    if (typeof error === "string" )
      error = { type: "Error", message: error } as Error

    setErrors((prev) => [...prev, error]);
  };

  const closeCurrentError = () => {
    setErrors((prev) => prev.filter((_, index) => index !== currentErrorIndex));
    setCurrentErrorIndex((prev) => Math.max(prev - 1, 0));
  };

  const clearErrors = () => {
    setErrors([]);
    setCurrentErrorIndex(0);
  };

  return (
    <ErrorContext.Provider
      value={{
        errors,
        currentErrorIndex,
        addError,
        closeCurrentError,
        clearErrors,
      }}
    >
      {children}
    </ErrorContext.Provider>
  );
};

export const useErrorContext = (): ErrorContextType => {
  const context = useContext(ErrorContext);
  if (!context) {
    throw new Error("useErrorContext must be used within an ErrorProvider");
  }
  return context;
};