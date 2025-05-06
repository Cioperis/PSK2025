import Discussions from "./components/Discussions.tsx";
import Chatting from "./components/Chatting.tsx";

const AppRoutes = [
    {
        path: "/",
        element: <Discussions />
    }, 
    {
        path: "/chat",
        element: <Chatting />
    }

];

export default AppRoutes;