import { List, Paper, Typography } from "@mui/material";
import ITodoList from "../models/ITodoList";
import TodoItem from "./TodoItem";

export default function TodoList(props: { list: ITodoList }) {

    return (
        <Paper elevation={3}>
            {
                (props.list.items.length > 0) ? (
                    <List>
                        {props.list.items.map((item: any) => (
                            <TodoItem item={item} key={item.position}></TodoItem>
                        ))}
                    </List>
                ) : (
                    <Typography>Todo list is empty...</Typography>
                )
            }
        </Paper>
    );
}