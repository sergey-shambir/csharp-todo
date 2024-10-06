import { Checkbox, ListItem, ListItemText } from "@mui/material";
import { TodoItemData } from "../api/TodoApi";

export default function TodoItemView(props: { item: TodoItemData }) {
    return (
        <ListItem>
            <ListItemText>{props.item.title}</ListItemText>
            <Checkbox checked={props.item.isComplete}></Checkbox>
        </ListItem>
    )
}