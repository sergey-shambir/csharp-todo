import { ListItem, ListItemButton, ListItemText } from "@mui/material";
import { TodoListData } from "../api/TodoApi";
import { Link as RouterLink } from "react-router-dom";


// TODO: Реализовать drag&drop средствами react-beautiful-dnd
// См. https://www.dhiwise.com/post/building-dynamic-interfaces-with-react-mui-drag-and-drop
export default function TodoListView(props: { list: TodoListData }) {
    return (
        <ListItem>
            <ListItemButton component={RouterLink} to={"/" + props.list.id}>
                <ListItemText>{props.list.name}</ListItemText>
            </ListItemButton>
        </ListItem>
    )
}