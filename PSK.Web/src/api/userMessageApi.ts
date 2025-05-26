import axiosInstance from "./axiosInstance.ts";
import {AxiosResponse} from "axios";

export interface UserMessage {
    id: string;
    userId: string;
    content: string;
    sendAt: Date;
    isRecruiting: boolean;
    isEnabled: boolean;
}

export interface ScheduleMessageRequest {
    days: string;
}

export interface UserMessageRequest {
    content: string;
    sendAt: Date;
    isRecurring: boolean;
}

export interface UserMessageResponse {
    message: string;
}

export const getAllUserMessages = async (): Promise<UserMessage[]> => {
    const response = await axiosInstance.get<UserMessage[]>(`/AutoMessage/GetAllMessages`);
    return response.data;
}

export const sendRandomMessage = async (): Promise<UserMessageResponse> => {
    const response = await axiosInstance.post<UserMessageResponse>('/AutoMessage/send-random');
    return response.data;
}

export const scheduleMessage = async (scheduleMessageRequest: ScheduleMessageRequest): Promise<UserMessageResponse> => {
    const response = await axiosInstance.post<UserMessageResponse>('/AutoMessage/schedule-messages', scheduleMessageRequest);
    return response.data;
}

export const scheduleCustomMessage = async (userMessageRequest: UserMessageRequest): Promise<UserMessage> => {
    const response = await axiosInstance.post<UserMessage>('/AutoMessage/schedule-custom-message', userMessageRequest);
    return response.data;
}

export const deleteCustomMessage = async (messageId: string): Promise<AxiosResponse> => {
    const response = await axiosInstance.delete(`/AutoMessage/${messageId}`);
    return response;
}