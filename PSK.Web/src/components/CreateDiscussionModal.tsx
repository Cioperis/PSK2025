import {Button, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader} from "reactstrap";
import {createDiscussion, DiscussionSchema} from "../api/discussionApi.ts";
import {useState} from "react";
import {useNavigate} from "react-router-dom";

interface CreateDiscussionModalProps {
    isOpen: boolean;
    setOpen: (p: boolean) => void;
}

const CreateDiscussionModal = ({ isOpen, setOpen }: CreateDiscussionModalProps) => {
    const [discussionName, setDiscussionName] = useState<string>("");

    const navigate = useNavigate();

    const handleCancel = () => {
        setOpen(false);
        setDiscussionName("");
    }

    const handleSubmit = () => {
        const newDiscussion: DiscussionSchema = {
            name: discussionName,
        }
        createDiscussion(newDiscussion)
            .then((discussion) => {navigate(`/discussion/${discussion.id}`)})
            .catch((error) => {console.error(error)});
    }

    return (
        <Modal isOpen={isOpen} onClose={handleCancel}>
            <ModalHeader>Create new discussion</ModalHeader>
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
}

export default CreateDiscussionModal;