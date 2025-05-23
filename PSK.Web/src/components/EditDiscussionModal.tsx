import {Button, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader} from "reactstrap";
import {Discussion, updateDiscussion} from "../api/discussionApi.ts";
import {useRef, useState} from "react";
import {useParams} from "react-router-dom";
import {toast} from "react-toastify";

interface EditDiscussionModalProps {
    isOpen: boolean;
    setOpen: (p: boolean) => void;
    setDiscussion: (p: Discussion) => void;
}

const EditDiscussionModal = ({ isOpen, setOpen, setDiscussion }: EditDiscussionModalProps) => {
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
            updatedAt: new Date()
        };

        updateDiscussion(newDiscussion)
            .then((discussion) => {
                setDiscussion(discussion);
                setOpen(false);
                toast.success("Discussion updated successfully");
            })
            .catch((error) => {
                if (error.response?.status === 409) {
                    pendingDiscussion.current = newDiscussion;
                    showConflictToast();
                } else {
                    console.error(error);
                    toast.error("Discussion updated failed");
                }
            });
    };

    const showConflictToast = () => {
        toast.error(
            <div>
                <p>This discussion was modified by another user.</p>
                <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                    <button
                        onClick={() => handleRetry()}
                        style={{ padding: '5px 10px', background: '#dc3545', color: 'white', border: 'none', borderRadius: '4px' }}
                    >
                        Overwrite Changes
                    </button>
                    <button
                        onClick={() => {
                            toast.dismiss();
                            pendingDiscussion.current = null;
                        }}
                        style={{ padding: '5px 10px', background: '#6c757d', color: 'white', border: 'none', borderRadius: '4px' }}
                    >
                        Cancel
                    </button>
                </div>
            </div>,
            {
                autoClose: false,
                closeButton: false,
            }
        );
    };

    const handleRetry = () => {
        if (!pendingDiscussion.current) return;
        console.log("retrying...2")

        updateDiscussion(pendingDiscussion.current)
            .then((discussion) => {
                setDiscussion(discussion);
                setOpen(false);
                toast.dismiss();
                toast.success("Discussion updated successfully");
            })
            .catch((error) => {
                if (error.response?.status === 409) {
                    toast.dismiss();
                    showConflictToast();
                } else {
                    toast.dismiss();
                    toast.error("An error occurred while updating the discussion");
                }
            });
    };

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