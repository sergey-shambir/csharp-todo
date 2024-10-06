import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import TodoListPage from './components/TodoListPage';
import HomePage from './components/HomePage';

const router = createBrowserRouter([
  {
    path: "/",
    Component: HomePage
  },
  {
    path: "/:listId",
    Component: TodoListPage
  }
]);

function App() {
  return (
    <RouterProvider router={router} />
  );
}

export default App;
