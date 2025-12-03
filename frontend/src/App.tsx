import {Container, Navbar} from 'react-bootstrap';
import {ToastContainer} from 'react-toastify';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import TodoTable from './features/todos/TodoTable';
import TodoForm from './features/todos/TodoForm';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
    },
  },
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <div className="min-vh-100 bg-light">
        {/* Navbar */}
        <Navbar bg="dark" variant="dark" expand="lg" className="mb-4 shadow">
          <Container>
            <Navbar.Brand href="#">
              VerbundPflegehilfe Task Manager
            </Navbar.Brand>
          </Container>
        </Navbar>

        {/* Main Content */}
        <Container>
          <div className="row justify-content-center">
            <div className="col-lg-10">
              <TodoForm/>
              <TodoTable/>
            </div>
          </div>
        </Container>

        {/* Toast Notifications */}
        <ToastContainer position="bottom-right" theme="colored"/>
      </div>
    </QueryClientProvider>
  );
}

export default App;