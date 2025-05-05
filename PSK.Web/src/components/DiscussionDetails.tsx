import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Discussion, getDiscussionById} from "../api/discussionApi.ts";
import {Button, Card, CardBody, CardText, CardTitle, Input, InputGroup} from "reactstrap";
import {getAllCommentsByDiscussionId, Comment, createComment, CommentSchema} from "../api/commentApi.ts";

const DiscussionDetails = () => {
    const {id} = useParams<{ id: string }>();
    const [discussion, setDiscussion] = useState<Discussion | null>(null);
    const [comments, setComments] = useState<Comment[]>([]);
    const [newCommentContent, setNewCommentContent] = useState<string>('');

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
        createComment(newComment).then(comment => {setComments([...comments, comment]);});
    }

    return(
        <>
            <div>
                <h1 className="my-3">{discussion?.name}</h1>
            </div>
            {comments?.map((comment: Comment) => (
                <Card key={comment.id}>
                    <CardBody>
                        <CardTitle><strong>Username</strong></CardTitle>
                        <CardText>
                            {comment.content}
                        </CardText>
                    </CardBody>
                </Card>
            ))}
            <InputGroup className="mt-3">
                <Input type="textarea" onChange={e => setNewCommentContent(e.target.value)}></Input>
                <Button color="secondary" outline onClick={handleCreateCommentClick}>
                    <img src="../../public/send.svg" alt="send"></img>
                </Button>
            </InputGroup>
        </>
    );
}

export default DiscussionDetails;