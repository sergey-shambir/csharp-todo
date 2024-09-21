import { Checkbox, ListItem, ListItemText } from "@mui/material";
import ITodoItem from "../models/ITodoItem";

export default function TodoItem(props: { item: ITodoItem }) {
    return (
        <ListItem>
            <ListItemText>{props.item.title}</ListItemText>
            <Checkbox checked={props.item.isComplete}></Checkbox>
        </ListItem>
    )
}