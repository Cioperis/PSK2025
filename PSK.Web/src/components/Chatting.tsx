import { useState } from "react";
import "./Chatting.css";


const Chatting = () => {
    
    const [isPatientPlaying, setPatientPlaying] = useState(false);
    const [isHelperPlaying, setHelperPlaying] = useState(false);

    const [showPatientPlayingImage, setShowPatientPlayingImage] = useState(false);
    const [showHelperPlayingImage, setShowHelperPlayingImage] = useState(false);

    const [expandedSide, setExpandedSide] = useState<null | 'patient' | 'helper'>(null);
    const [isPatientExpanded, setPatientExpanded] = useState<null | 'yes' | 'no'>(null);
    const [isHelperExpanded, setHelperExpanded] = useState<null | 'yes' | 'no'>(null);

    const [isConnectingPatient, setConnectingPatient] = useState(false);
    const [isConnectingHelper, setConnectingHelper] = useState(false);

    const [isConnected, setConnected] = useState(true);


    const handleButtonClick = (isPatient: boolean) => {
        if (isPatient) {
            if (!isPatientPlaying) {
                setPatientPlaying(true);
                setShowPatientPlayingImage(false);
                setExpandedSide('patient');
                setTimeout(() => {
                    setShowPatientPlayingImage(true);
                    setPatientExpanded('yes');
                    setConnectingPatient(true);
                }, 400);
            } else {
                setPatientPlaying(false);
                setShowPatientPlayingImage(false);
                setExpandedSide(null);
                setPatientExpanded('no');
                setTimeout(() => {
                    setPatientExpanded(null);
                }, 50);
                setTimeout(() => {
                    setConnectingPatient(false);
                }, 400);
            }
        } else {
            if (!isHelperPlaying) {
                setHelperPlaying(true);
                setShowHelperPlayingImage(false);
                setExpandedSide('helper');
                setTimeout(() => {
                    setShowHelperPlayingImage(true);
                    setHelperExpanded('yes');
                    setConnectingHelper(true);
                }, 400);
            } else {
                setHelperPlaying(false);
                setShowHelperPlayingImage(false);
                setExpandedSide(null);
                setHelperExpanded('no');
                setTimeout(() => {
                    setHelperExpanded(null);
                }, 50);
                setTimeout(() => {
                    setConnectingHelper(false);
                }, 400);
            }
        }        
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

            {/*Chatting window!!!!*/}

            <div className="chatting">
                <div className="chatBox">
                    <div className="input">
                        <div className="textDiv">
                            <input className="textField" type="textbox" placeholder="Type message..."/>
                        </div>
                        <div className="sendButtonDiv">
                            <button className="sendButton">

                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Chatting;