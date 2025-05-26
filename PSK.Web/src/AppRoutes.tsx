import Discussions from "./components/Discussions.tsx";
import Chatting from "./components/Chatting.tsx";
import DiscussionDetails from "./components/DiscussionDetails.tsx";
import Notifications from "./components/Notifications.tsx";

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
    },
    {
        path: "/notifications",
        element: <Notifications />
    }

];

export default AppRoutes;