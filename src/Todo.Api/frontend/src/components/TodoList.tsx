import { List, Paper, Typography } from "@mui/material";
import { TodoListDetailedData } from "../api/TodoApi";
import TodoItem from "./TodoItem";
import AddItemForm from "./AddItemForm";

export default function TodoList(props: { list: TodoListDetailedData }) {
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
            <AddItemForm />
        </Paper>
    );
}