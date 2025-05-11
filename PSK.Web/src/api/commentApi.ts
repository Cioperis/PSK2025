import axiosInstance from "./axiosInstance.ts";
import {AxiosResponse} from "axios";

export interface Comment {
    id: string;
    content: string;
    discussionId: string;
    updatedAt: Date;
}

export interface CommentSchema {
    content: string;
    discussionId: string;
}

export const getCommentById = async (id: string): Promise<Comment> => {
    const response = await axiosInstance.get<Comment>(`/Comment/${id}`);
    return response.data;
}

export const getAllComments = async (): Promise<Comment[]> => {
    const response = await axiosInstance.get<Comment[]>('/Comment/all');
    return response.data;
}

export const getAllCommentsByDiscussionId = async (discussionId: string): Promise<Comment[]> => {
    const response = await axiosInstance.get<Comment[]>(`/Comment/ofDiscussion/${discussionId}`);
    return response.data;
}

export const createComment = async (comment: CommentSchema): Promise<Comment> => {
    const response = await axiosInstance.post<Comment>('/Comment', comment);
    return response.data;
}

export const updateComment = async (comment: Comment): Promise<Comment> => {
    const response = await axiosInstance.put<Comment>('/Comment', comment);
    return response.data;
}

export const deleteComment = async (commentId: string): Promise<AxiosResponse> => {
    const response = await axiosInstance.delete(`/Comment/${commentId}`);
    return response;
}