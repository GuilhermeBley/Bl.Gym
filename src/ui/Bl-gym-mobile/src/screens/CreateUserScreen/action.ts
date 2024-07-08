import axios from "../../api/GymApi";

interface UserCreationResult{
    Success: boolean,
    Errors: string[]
}

export function handleCreateUser(
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phoneNumber: string | null
): Promise<UserCreationResult>{
    return axios.post('api/user', {
        firstName,
        lastName,
        email,
        password,
        phoneNumber,
    }).then(response => {

        if (response.status == 200)
            return {
                Success: true,
                Errors: []
            }

        if (response.status == 403)
            return {
                Success: false,
                Errors: ['Usuário já existente.']
            }

        return {
            Success: false,
            Errors: ['Falha ao criar usuário.']
        }
    })
}