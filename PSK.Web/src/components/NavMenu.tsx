import {Link} from "react-router-dom";
import {
  Collapse, Nav, Navbar, NavbarBrand, NavbarToggler, NavLink,
  Dropdown, DropdownToggle, DropdownMenu, DropdownItem, Spinner
} from "reactstrap";
import {useState, useEffect} from "react";
import axiosInstance from "../api/axiosInstance";
import {useAuth} from "../context/AuthContext";

const NavMenu = () => {

    const [isCollapsed, setCollapsed] = useState(true);
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [user, setUser] = useState<{ firstName: string; lastName: string; email: string } | null>(null);
    const [loading, setLoading] = useState(true);
    const auth = useAuth();

    useEffect(() => {
        axiosInstance.get("/User/Me")
            .then(res => setUser(res.data))
            .catch(() => setUser(null))
            .finally(() => setLoading(false));
    }, []);

    const toggleDropdown = () => setDropdownOpen(o => !o);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom bg-white">
                <NavbarBrand tag={Link} to="/">
                    <img src="/ico.png" alt="Well" style={{height: '50px'}}/>
                </NavbarBrand>
                <NavbarToggler onClick={() => setCollapsed(!isCollapsed)} className="mr-2" />
                <Collapse isOpen={!isCollapsed} navbar>
                    <Nav navbar className="me-auto">
                        <NavLink tag={Link} to="/" className="text-dark align-items-center">
                            <div>Discussions</div>
                        </NavLink>
                        <NavLink tag={Link} to="/chat" className="text-dark align-items-center">
                            <div>Chat</div>
                        </NavLink>
                        <NavLink tag={Link} to="https://www.youtube.com/watch?v=FjAaEiQfUZo" className="text-dark align-items-center">
                            <div>Sick Beats</div>
                        </NavLink>
                    </Nav>
                    <Nav navbar className="ms-auto">
                        <Dropdown isOpen={dropdownOpen} toggle={toggleDropdown}>
                            <DropdownToggle tag="button" className="btn btn-link nav-link" style={{textDecoration: 'none'}}>
                                {loading
                                    ? <Spinner size="sm" />
                                    : <span style={{
                                        display: 'inline-block',
                                        width: 32, height: 32,
                                        borderRadius: '50%',
                                        background: '#ddd',
                                        textAlign: 'center',
                                        lineHeight: '32px',
                                        fontWeight: 'bold',
                                        color: '#777',
                                        userSelect: 'none'
                                    }}>
                                        {user?.firstName.charAt(0).toUpperCase() ?? "?"}
                                      </span>
                                }
                            </DropdownToggle>
                            <DropdownMenu end>
                                {user && <>
                                    <DropdownItem header>{user.firstName} {user.lastName}</DropdownItem>
                                    <DropdownItem disabled>{user.email}</DropdownItem>
                                    <DropdownItem divider />
                                </>}
                                <DropdownItem onClick={auth.logout}>Logout</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </Nav>
                </Collapse>
            </Navbar>
        </header>
    );
}

export default NavMenu;