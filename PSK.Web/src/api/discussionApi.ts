import axiosInstance from "./axiosInstance.ts";
import {AxiosResponse} from "axios";

export interface Discussion {
    id: string;
    name: string;
    updatedAt: Date;
    userId: string;
}

export interface DiscussionSchema {
    name: string;
}

export const getDiscussionById = async (id: string): Promise<Discussion> => {
    const response = await axiosInstance.get<Discussion>(`/Discussion/${id}`);
    return response.data;
}

export const getAllDiscussions = async (): Promise<Discussion[]> => {
    const response = await axiosInstance.get<Discussion[]>('/Discussion/all');
    return response.data;
}

export const createDiscussion = async (discussion: DiscussionSchema): Promise<Discussion> => {
    const response = await axiosInstance.post<Discussion>('/Discussion', discussion);
    return response.data;
}

export const updateDiscussion = async (discussion: Discussion): Promise<Discussion> => {
    const response = await axiosInstance.put<Discussion>('/Discussion', discussion);
    return response.data;
}

export const deleteDiscussion = async (discussionId: string): Promise<AxiosResponse> => {
    const response = await axiosInstance.delete(`/Discussion/${discussionId}`);
    return response;
}