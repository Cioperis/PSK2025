import {
    Button,
    Card,
    CardBody,
    CardFooter,
    CardText,
    CardTitle,
    Input,
    Modal,
    ModalBody,
    ModalFooter,
    ModalHeader,
    Tooltip
} from "reactstrap";
import {useEffect, useState} from "react";
import {
    deleteCustomMessage,
    getAllUserMessages,
    scheduleCustomMessage, scheduleMessage, sendRandomMessage,
    UserMessage,
    UserMessageRequest
} from "../api/userMessageApi.ts";
import { format } from "date-fns";

const Notifications = () => {
    const [userMessages, setUserMessages] = useState<UserMessage[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [newUserMessageContent, setNewUserMessageContent] = useState("");
    const [newUserMessageTime, setNewUserMessageTime] = useState<string>("");
    const [newUserMessageDate, setNewUserMessageDate] = useState<Date>();
    const [tooltipOpen, setTooltipOpen] = useState(false);
    const [isEnableMotivationalModalOpen, setIsEnableMotivationalModalOpen] = useState(false);
    const [motivationalMessageDays, setMotivationalMessageDays] = useState<string>("0");

    useEffect(() => {
        getAllUserMessages().then(setUserMessages).catch(console.error);
    }, []);

    const toggleIsCreateModalOpen = () => setIsCreateModalOpen(!isCreateModalOpen);
    const toggleTooltip = () => setTooltipOpen(!tooltipOpen);
    const toggleMotivationalModal = () => setIsEnableMotivationalModalOpen(!isEnableMotivationalModalOpen);


    const handleSendTestMessage = () => {
        sendRandomMessage().then(() => alert("Message sent successfully"));
    }


    const handleTimeChange = (timeValue: string) => {
        setNewUserMessageTime(timeValue);

        if (timeValue) {
            const today = new Date();
            const [hours, minutes] = timeValue.split(':').map(Number);

            const combinedDate = new Date(
                today.getFullYear(),
                today.getMonth(),
                today.getDate(),
                hours,
                minutes
            );

            setNewUserMessageDate(combinedDate);
        }
    }

    const handleScheduleMotivationalMessages = () => {
        scheduleMessage({days: motivationalMessageDays})
            .then((umr) => alert(umr.message))
            .catch(console.error);

        setIsEnableMotivationalModalOpen(false);
        setMotivationalMessageDays("0");
    }

    const deleteCustomReminder = (id: string) => {
        deleteCustomMessage(id)
            .then(() => setUserMessages(userMessages.filter(um => um.id != id)))
            .catch(console.error);
    }

    const createCustomReminder = () => {
        if (!newUserMessageContent || !newUserMessageDate) {
            alert("Please enter both content and time");
            return;
        }

        const newCustomMessage: UserMessageRequest = {
            sendAt: newUserMessageDate,
            content: newUserMessageContent,
            isRecurring: true
        }

        scheduleCustomMessage(newCustomMessage).then((umr) => {
            alert("New reminder created successfully.");
            setUserMessages([...userMessages, umr])
        }).catch(console.error);

        toggleIsCreateModalOpen();
        setNewUserMessageContent("");
        setNewUserMessageTime("");
        setNewUserMessageDate(undefined);
    }

    return (
        <>
            <div className="py-3">
                <h3>Positive messages</h3>
                <Card className="w-50 shadow">
                    <CardBody>
                        <CardTitle tag="h5">Motivational</CardTitle>
                        <CardText>Motivational messages, when feeling down</CardText>
                    </CardBody>
                    <CardFooter>
                        <Button color="secondary" onClick={toggleMotivationalModal}>Enable</Button>
                        <Button
                            className="mx-2"
                            id="exampleMessageBtn"
                            color="info"
                            onClick={handleSendTestMessage}
                        >
                            Try Example
                        </Button>
                        <Tooltip
                            isOpen={tooltipOpen}
                            target="exampleMessageBtn"
                            toggle={toggleTooltip}
                        >
                            Sends example message to your email
                        </Tooltip>
                    </CardFooter>
                </Card>
            </div>
            <div className="py-3">
                <h3>Reminders</h3>
                <Button color="secondary" onClick={toggleIsCreateModalOpen} className="mb-3">+</Button>
                <div className="row">
                    {userMessages.map((message) => (
                        <div className="col-md-6 mb-3" key={message.id}>
                            <Card className="shadow">
                                <CardBody>
                                    <CardTitle tag="h5">Custom reminder</CardTitle>
                                    <CardText>{message.content}</CardText>
                                    <CardText>{format(new Date(message.sendAt), 'HH:mm')}</CardText>
                                </CardBody>
                                <CardFooter>
                                    <Button color="danger" onClick={() => deleteCustomReminder(message.id)}>Delete</Button>
                                </CardFooter>
                            </Card>
                        </div>
                    ))}
                </div>
            </div>

            <Modal isOpen={isCreateModalOpen} toggle={toggleIsCreateModalOpen}>
                <div className="p-3">
                    <h4>Create New Reminder</h4>
                    <Input
                        type="textarea"
                        placeholder="Enter your reminder content"
                        value={newUserMessageContent}
                        onChange={e => setNewUserMessageContent(e.target.value)}
                        className="mb-3"
                    />
                    <Input
                        type="time"
                        value={newUserMessageTime}
                        onChange={e => handleTimeChange(e.target.value)}
                        className="mb-3"
                    />
                    <Button color="primary" onClick={createCustomReminder}>
                        Create Reminder
                    </Button>
                </div>
            </Modal>

            <Modal isOpen={isEnableMotivationalModalOpen} toggle={toggleMotivationalModal}>
                <ModalHeader>
                    For how many days (1-30d.)
                </ModalHeader>
                <ModalBody>
                    <Input
                        type="number"
                        value={motivationalMessageDays}
                        onChange={e => setMotivationalMessageDays(e.target.value)}
                    />
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={handleScheduleMotivationalMessages}>Save</Button>
                    <Button onClick={toggleMotivationalModal}>Cancel</Button>
                </ModalFooter>

            </Modal>
        </>
    )
}

export default Notifications;