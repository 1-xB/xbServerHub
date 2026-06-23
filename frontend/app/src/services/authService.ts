import api from "./api";
import { AxiosError } from "axios";

export interface LoginDto {
    email: string;
    password: string;
}

export interface AuthResult {
    isSuccess: boolean;
    message: string;
    requiresTwoFactorAuth: boolean;
}

export const loginUser = async (credentials: LoginDto): Promise<AuthResult> => {
    try {
        const response = await api.post("auth/login", credentials);
        return response.data as AuthResult;
    }
    catch (error: unknown) {
        let errorMessage = "An error occurred during login.";
        if ((error as AxiosError)?.response?.data) {
            errorMessage = (error as AxiosError).response!.data as string;
        }
        return {
            isSuccess: false,
            message: errorMessage,
            requiresTwoFactorAuth: false,
        };
    }
}