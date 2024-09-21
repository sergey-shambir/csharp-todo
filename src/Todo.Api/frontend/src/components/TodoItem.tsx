import { Checkbox, ListItem, ListItemText } from "@mui/material";
import TodoItemData from "../models/TodoItem";

export default function TodoItem(props: { item: TodoItemData }) {
    return (
        <ListItem>
            <ListItemText>{props.item.title}</ListItemText>
            <Checkbox checked={props.item.isComplete}></Checkbox>
        </ListItem>
    )
}