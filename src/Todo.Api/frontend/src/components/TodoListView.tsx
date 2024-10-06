import { ListItem, ListItemButton, ListItemText } from "@mui/material";
import { TodoListData } from "../api/TodoApi";
import { Link as RouterLink } from "react-router-dom";


export default function TodoListView(props: { list: TodoListData }) {
    return (
        <ListItem>
            <ListItemButton component={RouterLink} to={"/" + props.list.id}>
                <ListItemText>{props.list.name}</ListItemText>
            </ListItemButton>
        </ListItem>
    )
}