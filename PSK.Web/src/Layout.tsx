import {Container} from "reactstrap";
import NavMenu from "./components/NavMenu";
import {ReactNode} from "react";

interface LayoutProps {
    children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
    return (
        <div>
            <NavMenu/>
            <Container fluid className="px-5" tag="main">
                {children}
            </Container>
        </div>
    );
}

export default Layout;