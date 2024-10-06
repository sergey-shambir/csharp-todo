import { Alert, Box, Container, List, Paper, Typography } from "@mui/material";
import { TodoApi, TodoItemData, TodoListDetailedData } from "../api/TodoApi";
import TodoItemView from "./TodoItemView";
import AddItemForm from "./AddItemForm";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

type TodoListPageData = {
    isLoading: boolean,
    list?: TodoListDetailedData,
    error?: string
}

export default function TodoListPage() {
    const { listId } = useParams();
    const [data, setData] = useState<TodoListPageData>({
        isLoading: true
    });

    useEffect(() => {
        TodoApi.getTodoList(parseInt(listId!)).then(
            list => setData({
                isLoading: false,
                list: list
            }),
            error => setData({
                isLoading: false,
                error: error.message
            })
        );
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [listId]);

    return (
        <Container maxWidth="sm">
            <Typography variant="h4" textAlign="center" gutterBottom>{data.list?.name}</Typography>
            <Paper elevation={3}>
                {
                    (data.list && data.list.items.length > 0) ? (
                        <List>
                            {data.list.items.map((item: TodoItemData) => (
                                <TodoItemView item={item} key={item.position}></TodoItemView>
                            ))}
                        </List>
                    ) : (
                        <Box padding={2}>
                            <Typography>No tasks yet...</Typography>
                        </Box>
                    )
                }
                <AddItemForm
                    listId={data.list?.id}
                    disabled={data.isLoading}
                    onAdded={(title: string) => {
                        setData({
                            ...data,
                            list: {
                                ...data.list!,
                                items: [
                                    ...data.list!.items,
                                    {
                                        position: data.list!.items.length,
                                        title: title,
                                        isComplete: false
                                    }
                                ]
                            }
                        })
                    }}
                />
                {data.error ? (
                    <Alert severity="error">{data.error}</Alert>
                ) : null}
            </Paper>
        </Container>
    );
}