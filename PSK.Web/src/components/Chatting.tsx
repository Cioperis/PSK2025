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

    const [isConnecting, setConnecting] = useState(false);

    const handleButtonClick = (isPatient: boolean) => {
        if (isPatient) {
            if (!isPatientPlaying) {
                setPatientPlaying(true);
                setShowPatientPlayingImage(false);
                setExpandedSide('patient');
                setTimeout(() => {
                    setShowPatientPlayingImage(true);
                    setPatientExpanded('yes');
                    setConnecting(true);
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
                    setConnecting(false);
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
                    setConnecting(true);
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
                    setConnecting(false);
                }, 400);
            }
        }        
    };

    

    return(
        <div className="rootDiv">
            <div className={`choiceDiv patientDiv ${expandedSide === 'patient' ? 'expand' : expandedSide === 'helper' ? 'shrink' : ''} 
                ${isHelperExpanded === 'yes' ? 'hidden' : isHelperExpanded === 'no' ? 'shrink' : ''}`}>
                <button className={`connectButton ${isPatientPlaying ? 'playing' : ''}`} onClick={() => handleButtonClick(true)}>
                    <img className={`notPlaying ${isPatientPlaying ? 'animate' : ''} ${showPatientPlayingImage ? 'invisible' : ''}`}  src="./../../public/playButton.png" alt="playPng"></img>
                    <img className={`playingImage ${showPatientPlayingImage ? 'visible' : ''}`} src="./../../public/playing.png" alt="playing"></img>
                </button>
                <div className="loadingHeader">
                    <h5 className={isConnecting ? 'loadingInvisible' : ''}>Connect as patient</h5>
                    <div className={`loading ${isConnecting ? '' : 'loadingInvisible'}`}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                </div>
            </div>
            <div className={`choiceDiv helperDiv ${expandedSide === 'helper' ? 'expand' : expandedSide === 'patient' ? 'shrink' : ''} 
                ${isPatientExpanded === 'yes' ? 'hidden' : isPatientExpanded === 'no' ? 'shrink' : ''}`}>
                <button className={`connectButton ${isHelperPlaying ? 'playing' : ''}`} onClick={() => handleButtonClick(false)}>
                    <img className={`notPlaying ${isHelperPlaying ? 'animate' : ''} ${showHelperPlayingImage ? 'invisible' : ''}`} src="./../../public/playButton.png" alt="playPng"></img>
                    <img className={`playingImage ${showHelperPlayingImage ? 'visible' : ''}`} src="./../../public/playing.png" alt="playing"></img>
                </button>
                <div className="loadingHeader">
                    <h5 className={isConnecting ? 'loadingInvisible' : ''}>Connect as helper</h5>
                    <div className={`loading ${isConnecting ? '' : 'loadingInvisible'}`}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Chatting;