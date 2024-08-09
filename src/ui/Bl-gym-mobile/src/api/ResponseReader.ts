import { AxiosResponse } from "axios";

interface ErrorsDictionary {
    [key: string]: string;
}

const ErrorsDictionaryPtBr : ErrorsDictionary = {
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

const DefaultErrorMessage = "Erro ao executar operação.";

interface GymApiResponse<T = any> {
    Data: T,
    Errors: string[],
    ContainsError: boolean
}

function getErrorMessage(key: string): string | undefined {
    if (key in ErrorsDictionaryPtBr) {
        return ErrorsDictionaryPtBr[key];
    } else {
        return DefaultErrorMessage;
    }
}

function TryGetResultFromResponse<T = any>(
    response: AxiosResponse<T, any> | undefined | null
){
    if (response === null || response === undefined)
        return {
            Data: null,
            Errors: [DefaultErrorMessage],
            ContainsError: true
        } as GymApiResponse<T>

    if (response.status >= 200 && response.status < 300)
        return {
            Data: response.data,
            Errors: [],
            ContainsError: false
        } as GymApiResponse<T>

    if (!Array.isArray(response.data))
        return {
            Data: response.data,
            Errors: [DefaultErrorMessage],
            ContainsError: true
        } as GymApiResponse<T>
    
    var errors: string[] = []
    response.data.forEach(d => {
        if (typeof d.message !== "string")
            return;

        return getErrorMessage(d.message)
    })

    return {
        Data: response.data,
        Errors: errors
    } as GymApiResponse<T>
}

export default TryGetResultFromResponse;