import { AxiosResponse } from "axios";

const ErrorsDictionaryPtBr = {
    "Unauthorized": "Usuário não autenticado.",
    "Forbbiden": "Usuário não autorizado.",
    "UserIsntMemberOfThisGym": "Usuário não é membro dessa academia.",
    "NotFound": "Não foi encontrado.",
    "Conflict": "Operação já realizada ou cadastrada.",
    "UserAlreadyHasATrainingInProgressInThisGym": "Já existe um treino em progresso.",
    "ThisEntityWasAlreadyUpdateByAnotherSource": "Este item já foi atualizado por outro local.",
    "BadRequest": "Falha na requisição.",
    "InvalidEmail": "E-mail inválido.",
    "InvalidStringLength": "Tamanho inválido de campo.",
    "InvalidPassword": "Senha inválida.",
    "InvalidFirstName": "Primeiro nome inválido.",
    "InvalidLastName": "Sobrenome inválido.",
    "InvalidPhoneNumber": "Número de telefone inválido.",
    "InvalidTrainingSectionName": "Nome da seção de treino está inválida.",
    "InvalidSetOfTrainingSections": "Conjunto de treino inválido.",
    "InvalidSetValue": "Valor de conjunto de treino inválido.",
    "ItsRequiredAtLeastOneExerciseForSection": "É necessário um exercicío por sessão.",
    "InvalidDaysCount": "Quantidade de dias inválido.",
    "ThisUserSheetIsntStarted": "Ficha de treino não iniciada.",
    "UserIsLocked": "Usuário bloqueado.",
    "UserAlreadyLoggedInGym": "Usuário já registrado na academia.",
}

interface GymApiResponse {
    Data: any,
    Errors: string[]
}

function TryGetResultFromResponse(
    response: AxiosResponse | null
) {
    if (response == null)
        return {
            Data: null,
            Errors: []
        } as GymApiResponse

    if (response.status >= 200 || response.status < 300)
        return {
            Data: response.data,
            Errors: []
        } as GymApiResponse

    if (!Array.isArray(response.data))
        return {
            Data: response.data,
            Errors: []
        } as GymApiResponse
    
    var errors: string[] = []
    response.data.forEach(d => {
        if (typeof d.message !== "string")
            return;

        
    })

    return {
        Data: response.data,
        Errors: errors
    } as GymApiResponse
}

export default TryGetResultFromResponse;