import api from "./api";
import { AxiosError } from "axios";

export interface LoginDto {
    email: string;
    password: string;
}

export interface AuthResult {
    isSuccess: boolean;
    message: string;
    requiresTwoFactor: boolean;
}

export interface LoginWith2FADto {
    code: string;
    rememberDevice: boolean;
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
            requiresTwoFactor: false,
        };
    }
}

export const loginWith2FA = async (data: LoginWith2FADto): Promise<AuthResult> => {
    try {
        const response = await api.post("auth/login-with-authenticator", data);
        return response.data as AuthResult;
    }
    catch (error: unknown) {
        let errorMessage = "An error occurred during 2FA login.";
        if ((error as AxiosError)?.response?.data) {
            errorMessage = (error as AxiosError).response!.data as string;
        }
        return {
            isSuccess: false,
            message: errorMessage,
            requiresTwoFactor: false,
        };
    }
}

export const checkAuthStatus = async (): Promise<boolean> => {
    try {
        await api.get("user/me");
        return true;
    }
    catch {
        return false;
    }
}