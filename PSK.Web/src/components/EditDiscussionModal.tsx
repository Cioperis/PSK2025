import {Button, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader} from "reactstrap";
import {Discussion, updateDiscussion} from "../api/discussionApi.ts";
import {useRef, useState} from "react";
import {useParams} from "react-router-dom";
import {toast} from "react-toastify";

interface EditDiscussionModalProps {
    userId: string;
    isOpen: boolean;
    setOpen: (p: boolean) => void;
    setDiscussion: (p: Discussion) => void;
}

const EditDiscussionModal = ({ isOpen, setOpen, setDiscussion, userId }: EditDiscussionModalProps) => {
    const {id} = useParams<{ id: string }>();
    const [discussionName, setDiscussionName] = useState<string>("");
    const pendingDiscussion = useRef<Discussion | null>(null);

    const handleCancel = () => {
        setOpen(false);
        setDiscussionName("");
    };

    const handleSubmit = () => {
        if (!id || discussionName.length === 0) {
            toast.error("Please enter a valid discussion name");
            return;
        }

        const newDiscussion: Discussion = {
            id: id,
            name: discussionName,
            updatedAt: new Date(),
            userId: userId,
        }

        updateDiscussion(newDiscussion)
            .then((discussion) => {setDiscussion(discussion); setOpen(false);})
            .catch((error) => {console.error(error)});
    }

    return (
        <Modal isOpen={isOpen} onClose={handleCancel}>
            <ModalHeader toggle={handleCancel}>Update discussion</ModalHeader>
            <ModalBody>
                <FormGroup>
                    <Label>Name</Label>
                    <Input value={discussionName}
                           onChange={e => setDiscussionName(e.target.value)}
                    ></Input>
                </FormGroup>
            </ModalBody>
            <ModalFooter>
                <Button color="primary" onClick={handleSubmit}>Submit</Button>
                <Button color="secondary" onClick={handleCancel}>Cancel</Button>
            </ModalFooter>
        </Modal>
    );
};

export default EditDiscussionModal;