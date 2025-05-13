import Discussions from "./components/Discussions.tsx";
import Chatting from "./components/Chatting.tsx";
import DiscussionDetails from "./components/DiscussionDetails.tsx";

const AppRoutes = [
    {
        path: "/",
        element: <Discussions />
    }, 
    {
        path: "/chat",
        element: <Chatting />
    },
    {
        path: "/discussion/:id",
        element: <DiscussionDetails />
    }

];

export default AppRoutes;