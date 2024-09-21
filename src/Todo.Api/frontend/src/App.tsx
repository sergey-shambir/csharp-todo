import { useState } from 'react';
import TodoListData from './models/ITodoList';
import TodoList from './components/TodoList';
import { Container, Typography } from '@mui/material';

function App() {
  const [list, setList] = useState<TodoListData>({
    id: 1581,
    name: "TODO list #1",
    items: [
      { position: 0, title: "Cleanup the room", isComplete: true },
      { position: 1, title: "Feed the hamster", isComplete: false }
    ]
  })

  return (
    <Container maxWidth="sm">
      <Typography variant="h4" textAlign="center" gutterBottom>To-Do List</Typography>
      <TodoList list={list}></TodoList>
    </Container>
  );
}

export default App;
