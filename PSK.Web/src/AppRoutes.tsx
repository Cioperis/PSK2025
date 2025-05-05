import Discussions from "./components/Discussions.tsx";
import DiscussionDetails from "./components/DiscussionDetails.tsx";

const AppRoutes = [
    {
        path: "/",
        element: <Discussions />
    },
    {
        path: "/discussion/:id",
        element: <DiscussionDetails />
    }

];

export default AppRoutes;