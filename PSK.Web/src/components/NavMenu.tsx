import {Link} from "react-router-dom";
import {Collapse, Nav, Navbar, NavbarBrand, NavbarToggler, NavLink} from "reactstrap";
import {useState} from "react";

const NavMenu = () => {

    const [isCollapsed, setCollapsed] = useState(true);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom bg-white">
                <NavbarBrand tag={Link} to="/">
                    <img src="/ico.png" alt="Well" style={{height: '50px'}}/>
                </NavbarBrand>
                <NavbarToggler onClick={() => setCollapsed(!isCollapsed)} className="mr-2" />
                <Collapse isOpen={!isCollapsed} navbar>
                    <Nav navbar>
                        <NavLink tag={Link} to="/" className="text-dark align-items-center">
                            <div>Discussions</div>
                        </NavLink>
                    </Nav>
                    <Nav navbar>
                        <NavLink tag={Link} to="/chat" className="text-dark align-items-center">
                            <div>Chat</div>
                        </NavLink>
                    </Nav>
                    <Nav navbar>
                        <NavLink tag={Link} to="https://www.youtube.com/watch?v=FjAaEiQfUZo" className="text-dark align-items-center">
                            <div>Sick Beats</div>
                        </NavLink>
                    </Nav>
                </Collapse>
            </Navbar>
        </header>
    );
}

export default NavMenu;