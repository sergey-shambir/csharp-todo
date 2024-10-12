import { Checkbox, ListItem, ListItemText } from "@mui/material";
import { TodoItemData } from "../api/TodoApi";

type TodoItemViewProps = {
    item: TodoItemData
}

export default function TodoItemView(props: TodoItemViewProps) {
    return (
        <ListItem>
            <ListItemText>{props.item.title}</ListItemText>
            <Checkbox checked={props.item.isComplete}></Checkbox>
        </ListItem>
    )
}