import { useState, useRef, useEffect } from "react";
import "./Chatting.css";
import * as signalR from "@microsoft/signalr";


type Message = {
    id: string;
    text: string;
    date: string;
    sender: 'user' | 'other' | 'system';
};

const Chatting = () => {

    useEffect(() => {
        return () => {
            gracefulShutdown();
        };
    }, []);

    useEffect(() => {
        const handleUnload = () => {
            gracefulShutdown();
        };
        
        window.addEventListener('unload', handleUnload);
        
        return () => window.removeEventListener('unload', handleUnload);
    }, []);

    const gracefulShutdown = () => {
        if(connectionRef.current){
            connectionRef.current.stop();
            connectionRef.current = null;
            chatIdRef.current = null;
        }
    }

    const [messages, setMessages] = useState<Message[]>([]);
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const chatIdRef = useRef<string | null>(null);

    const addMessage = (msgWithoutId: Omit<Message, 'id'>) => {
        const newMsg: Message = {
          ...msgWithoutId,
          id: crypto.randomUUID()
        };
        setMessages(prev => [...prev, newMsg]);
    };
    const getMessageClass = (sender: number | string) => {
        if (sender === 'user') return 'fromUser';
        if (sender === 'other') return 'fromOther';
        return 'fromSystem';
    };

    const setupConnectionHandlers = (connection: signalR.HubConnection) => {
        connection.on("ReceiveMessage", (senderId: string, message: string, date: Date) => {
            const parsedDate = new Date(date);
            addMessage({
                text: message,
                date: parsedDate.toLocaleTimeString(),
                sender: 'other'
            });
        });

        connection.on("ReceiveSystemMessage", (message: string, date: Date) => {
            if(chatIdRef.current != null){
                const parsedDate = new Date(date); 
                addMessage({
                    text: message,
                    date: parsedDate.toLocaleTimeString(),
                    sender: 'system'
                });
            }
        });

        connection.on("ReceiveChatId", (id: string) => {
            chatIdRef.current = id;
            setConnected(true);
            setPartnerConnected(true);
        });

        connection.on("ForceDisconnect", () => {
            console.log("disconnected g!!");
            connection.stop().catch(err => console.error("Disconnect error:", err));
            setPartnerConnected(false);
        });
    };

    const connectToHub = async (userType: 'Patient' | 'Helper') => {
        if (connectionRef.current) await connectionRef.current.stop();
        
        try {
            const newConnection = new signalR.HubConnectionBuilder()
                .withUrl(`https://localhost:7262/chatHub?userType=${userType}`)
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Debug)
                .build();

            connectionRef.current = newConnection;
            setupConnectionHandlers(connectionRef.current);
            await connectionRef.current.start();
        } catch (err) {
            console.error("[SignalR] Connection failed:", err);
            if (userType === 'Patient') {
                resetPatientState();
            } else {
                resetHelperState();
            }
        }
    };
    
    const [isPatientPlaying, setPatientPlaying] = useState(false);
    const [isHelperPlaying, setHelperPlaying] = useState(false);

    const [showPatientPlayingImage, setShowPatientPlayingImage] = useState(false);
    const [showHelperPlayingImage, setShowHelperPlayingImage] = useState(false);

    const [expandedSide, setExpandedSide] = useState<null | 'patient' | 'helper'>(null);
    const [isPatientExpanded, setPatientExpanded] = useState<null | 'yes' | 'no'>(null);
    const [isHelperExpanded, setHelperExpanded] = useState<null | 'yes' | 'no'>(null);

    const [isConnectingPatient, setConnectingPatient] = useState(false);
    const [isConnectingHelper, setConnectingHelper] = useState(false);

    const [isConnected, setConnected] = useState(false);
    const [isPartnerConnected, setPartnerConnected] = useState(false);

    const handleButtonClick = async (isPatient: boolean) => {
        if (isPatient) {
            if (!isPatientPlaying) {
                setPatientPlaying(true);
                setShowPatientPlayingImage(false);
                setExpandedSide('patient');
                await new Promise(resolve => setTimeout(resolve, 400));
                setShowPatientPlayingImage(true);
                setPatientExpanded('yes');
                setConnectingPatient(true);
                await connectToHub('Patient');
            } else {
                setPatientPlaying(false);
                setShowPatientPlayingImage(false);
                setExpandedSide(null);
                setPatientExpanded('no');

                if (connectionRef.current) {
                    try {
                        await connectionRef.current.stop();
                        connectionRef.current = null;
                    } catch (err) {
                        console.error("Disconnection error:", err);
                    }
                }
                
                setTimeout(() => {
                    setPatientExpanded(null);
                }, 50);
                setTimeout(() => {
                    setConnectingPatient(false);
                }, 450);
            }
        } else {
            if (!isHelperPlaying) {
                setHelperPlaying(true);
                setShowHelperPlayingImage(false);
                setExpandedSide('helper');
                await new Promise(resolve => setTimeout(resolve, 400));
                setShowHelperPlayingImage(true);
                setHelperExpanded('yes');
                setConnectingHelper(true);
                await connectToHub('Helper');
            } else {
                setHelperPlaying(false);
                setShowHelperPlayingImage(false);
                setExpandedSide(null);
                setHelperExpanded('no');

                if (connectionRef.current) {
                    try {
                        await connectionRef.current.stop();
                        connectionRef.current = null;
                    } catch (err) {
                        console.error("Disconnection error:", err);
                    }
                }

                setTimeout(() => {
                    setHelperExpanded(null);
                }, 50);
                setTimeout(() => {
                    setConnectingHelper(false);
                }, 450);
            }
        }        
    };

    const [message, setMessage] = useState('');
    const sendMessage = async () => {
        if (message.trim() == '' || !isPartnerConnected) return;

        try{
            addMessage({ 
                text: message, 
                date: new Date().toLocaleTimeString(), 
                sender: 'user'
            });
            await connectionRef.current!.invoke("SendMessage", chatIdRef.current, message);
            setMessage('');
        } catch (error) {
            console.error("Error sending message:", error);
        }
    };

    const leaveChat = async () => {
        if (connectionRef.current) {
            await connectionRef.current.stop();
            connectionRef.current = null;
            chatIdRef.current = null;
        }

        if(isConnectingPatient){
            resetPatientState();
        } else{
            resetHelperState();
        }

        setMessages([]);
        setPartnerConnected(false);
        setConnected(false);
    };
    
    const resetPatientState = () => {
        setPatientPlaying(false);
        setShowPatientPlayingImage(false);
        setExpandedSide(null);
        setPatientExpanded(null);
        setConnectingPatient(false);
    };

    const resetHelperState = () => {
        setHelperPlaying(false);
        setShowHelperPlayingImage(false);
        setExpandedSide(null);
        setHelperExpanded(null);
        setConnectingHelper(false);
    };

    return(
        <div className="rootDiv">
            <div className={`choiceDiv patientDiv ${isConnected ? 'invisible' : ''} ${expandedSide === 'patient' ? 'expand' : expandedSide === 'helper' ? 'shrink' : ''} 
                ${isHelperExpanded === 'yes' ? 'invisible' : isHelperExpanded === 'no' ? 'shrink' : ''}`}>
                <button className={`connectButton ${isPatientPlaying ? 'playing' : ''}`} onClick={() => handleButtonClick(true)}>
                    <img className={`notPlaying ${isPatientPlaying ? 'animate' : ''} ${showPatientPlayingImage ? 'invisible' : ''}`}  src="./../../public/playButton.png" alt="playPng"></img>
                    <img className={`playingImage ${showPatientPlayingImage ? 'visible' : ''}`} src="./../../public/playing.png" alt="playing"></img>
                </button>
                <div className="loadingHeader">
                    <h5 className={isConnectingPatient ? 'invisible' : ''}>Connect as patient</h5>
                    <div className={`loading ${isConnectingPatient ? '' : 'invisible'}`}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                </div>
            </div>
            <div className={`choiceDiv helperDiv ${isConnected ? 'invisible' : ''} ${expandedSide === 'helper' ? 'expand' : expandedSide === 'patient' ? 'shrink' : ''} 
                ${isPatientExpanded === 'yes' ? 'invisible' : isPatientExpanded === 'no' ? 'shrink' : ''}`}>
                <button className={`connectButton ${isHelperPlaying ? 'playing' : ''}`} onClick={() => handleButtonClick(false)}>
                    <img className={`notPlaying ${isHelperPlaying ? 'animate' : ''} ${showHelperPlayingImage ? 'invisible' : ''}`} src="./../../public/playButton.png" alt="playPng"></img>
                    <img className={`playingImage ${showHelperPlayingImage ? 'visible' : ''}`} src="./../../public/playing.png" alt="playing"></img>
                </button>
                <div className="loadingHeader">
                    <h5 className={isConnectingHelper ? 'invisible' : ''}>Connect as helper</h5>
                    <div className={`loading ${isConnectingHelper ? '' : 'invisible'}`}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                </div>
            </div>

            <div className={`chatting ${isConnected ? '' : 'invisible'}`}>
                <div className="chatBox">
                    <div className="headerOfChat">
                        <div className={`connectionStatus ${isPartnerConnected ? 'connectionStatusOn' : 'connectionStatusOff'}`}></div>
                        <h4>{isConnectingPatient ? 'Connected as patient' : 'Connected as helper'}</h4>
                        <button className="disconnectButton" onClick={leaveChat}>Leave chat</button>
                    </div>
                    <div className="chatPart">
                        <div className="messagesContainer">
                            {messages.map(msg => (
                            <div key={msg.id} className={`message ${getMessageClass(msg.sender)}`}>
                                <span className="messageText">{msg.text}</span>
                                <span className="messageDate">{msg.date}</span>
                            </div>
                            ))}
                        </div>
                    </div>
                    <div className="input">
                        <div className="textDiv">
                            <textarea className="textField" placeholder="Type message..."
                             value={message}
                             onChange={(e) => setMessage(e.target.value)}
                             onKeyDown={(e) => {
                                if (e.key === 'Enter' && !e.shiftKey) {
                                    e.preventDefault();
                                    sendMessage();
                                }
                            }}/>
                        </div>
                        <div className="sendButtonDiv">
                            <button className="sendButton" onClick={sendMessage}>
                                <img className="sendButtonImg" src="./../../public/send.png" alt="sendImage"></img>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Chatting;