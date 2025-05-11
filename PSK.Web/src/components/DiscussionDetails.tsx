import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Discussion, getDiscussionById} from "../api/discussionApi.ts";
import {Button, Card, CardBody, CardText, CardTitle, Input, InputGroup} from "reactstrap";
import {getAllCommentsByDiscussionId, Comment, createComment, CommentSchema} from "../api/commentApi.ts";
import {formatDistanceToNow} from "date-fns";
import EditDiscussionModal from "./EditDiscussionModal.tsx";

const DiscussionDetails = () => {
    const {id} = useParams<{ id: string }>();
    const [discussion, setDiscussion] = useState<Discussion | null>(null);
    const [comments, setComments] = useState<Comment[]>([]);
    const [newCommentContent, setNewCommentContent] = useState<string>('');
    const [isEditDiscussionModalOpen, setIsEditDiscussionModalOpen] = useState<boolean>(false);

    useEffect(() => {
        if (id){
            getDiscussionById(id).then(setDiscussion);
            getAllCommentsByDiscussionId(id).then(setComments);
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

    const handleEditDiscussionClick = () => {
        setIsEditDiscussionModalOpen(true);
    }

    return(
        <>
            <div className="d-flex justify-content-between align-items-center my-3">
                <h1>{discussion?.name}</h1>
                <Button onClick={handleEditDiscussionClick}>
                    Edit
                </Button>
            </div>
            {comments?.map((comment: Comment) => (
                <Card key={comment.id}
                      className="shadow-sm"
                >
                    <CardBody>
                        <CardTitle><strong>Username</strong></CardTitle>
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
            />
        </>
    );
}

export default DiscussionDetails;