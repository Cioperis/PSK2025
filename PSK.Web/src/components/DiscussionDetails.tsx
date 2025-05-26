import {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {deleteDiscussion, Discussion, getDiscussionById} from "../api/discussionApi.ts";
import {Button, Card, CardBody, CardText, CardTitle, Input, InputGroup} from "reactstrap";
import {getAllCommentsByDiscussionId, Comment, createComment, CommentSchema} from "../api/commentApi.ts";
import {formatDistanceToNow} from "date-fns";
import EditDiscussionModal from "./EditDiscussionModal.tsx";
import {ToastContainer} from "react-toastify";
import axiosInstance from "../api/axiosInstance.ts";

const DiscussionDetails = () => {
    const {id} = useParams<{ id: string }>();
    const [discussion, setDiscussion] = useState<Discussion | null>(null);
    const [comments, setComments] = useState<Comment[]>([]);
    const [newCommentContent, setNewCommentContent] = useState<string>('');
    const [isEditDiscussionModalOpen, setIsEditDiscussionModalOpen] = useState<boolean>(false);
    const [userId, setUserId] = useState<string>("");

    const navigate = useNavigate();

    useEffect(() => {
        if (id){
            getDiscussionById(id).then(setDiscussion);
            getAllCommentsByDiscussionId(id).then(setComments);

            axiosInstance.get("/User/Me")
                .then(res => setUserId(res.data.id));
        }
    }, [id])

    const handleCreateCommentClick = () => {
        if (!id)
            return;

        const newComment: CommentSchema = {
            content: newCommentContent,
            discussionId: id
        }
        createComment(newComment).then(comment => {
            setComments([...comments, comment]);
            setNewCommentContent('');
        });
    }

    const handleDeleteDiscussionClick = () => {
        if (!id)
            return;

        deleteDiscussion(id).then(() => navigate("/"))
    }

    const handleEditDiscussionClick = () => {
        setIsEditDiscussionModalOpen(true);
    }

    return(
        <>
            <div className="d-flex justify-content-between align-items-center my-3">
                <h1>{discussion?.name}</h1>
                {userId === discussion?.userId && (
                    <div>
                        <Button onClick={handleEditDiscussionClick}>
                            Edit
                        </Button>
                        <Button color="danger" className="mx-2" onClick={handleDeleteDiscussionClick}>
                            Delete
                        </Button>
                    </div>
                )}
            </div>
            {comments?.map((comment: Comment) => (
                <Card key={comment.id}
                      className="shadow-sm"
                >
                    <CardBody>
                        <CardTitle><strong>{comment.username}</strong></CardTitle>
                        <CardText className="d-flex justify-content-between align-items-center">
                            {comment.content}
                            <small>
                                {formatDistanceToNow(comment.updatedAt, { addSuffix: true })}
                            </small>
                        </CardText>
                    </CardBody>
                </Card>
            ))}
            <InputGroup className="mt-3">
                <Input type="textarea"
                       onChange={e => setNewCommentContent(e.target.value)}
                       style={{ background: "#ddf7fa" }}
                ></Input>
                <Button color="secondary" outline onClick={handleCreateCommentClick}>
                    <img src="../../public/send.svg" alt="send"></img>
                </Button>
            </InputGroup>

            <EditDiscussionModal isOpen={isEditDiscussionModalOpen}
                                 setOpen={setIsEditDiscussionModalOpen}
                                 setDiscussion={setDiscussion}
                                 userId={userId}
            />

            <ToastContainer/>
        </>
    );
}

export default DiscussionDetails;