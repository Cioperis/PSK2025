import {useEffect, useState } from "react";
import {getAllDiscussions, Discussion} from "../api/discussionApi.ts";
import CreateDiscussionModal from "./CreateDiscussionModal.tsx";
import {Button, Card, CardBody, CardText, CardTitle} from "reactstrap";
import {formatDistanceToNow} from "date-fns";
import {useNavigate} from "react-router-dom";

const Discussions = () => {
    const [discussions, setDiscussions] = useState<Discussion[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        getAllDiscussions().then(setDiscussions).catch(console.error);
    },[])

    const handleCardClick = (id: string) => {
        navigate(`/discussion/${id}`);
    };

    const handleCreateDiscussionClick = () => {
        setIsCreateModalOpen(true);
    }

    return(
        <>
            <div className="d-flex justify-content-between align-items-center my-3">
                <h1>Discussion groups</h1>
                <Button onClick={handleCreateDiscussionClick}>
                    Create new
                </Button>
            </div>
            {discussions.map((discussion) => (
                <Card
                    key={discussion.id}
                    className="my-2 shadow-sm"
                    onClick={() => handleCardClick(discussion.id)}
                    style={{ cursor: "pointer", background: "#ddf7fa" }}
                >
                    <CardBody>
                        <CardTitle tag="h5">{discussion.name}</CardTitle>
                        <CardText>
                            <small className={`text-muted`}>
                                Created {formatDistanceToNow(discussion.updatedAt, { addSuffix: true })}
                            </small>
                        </CardText>
                    </CardBody>
                </Card>
            ))}

            <CreateDiscussionModal isOpen={isCreateModalOpen} setOpen={setIsCreateModalOpen} />
        </>
    )
}

export default Discussions;