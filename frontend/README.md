# VerbundPflegehilfe - Frontend

A small React + Vite frontend for the VerbundPflegehilfe Task Manager.

This project uses React, TypeScript, React-Bootstrap, React Query and Axios.

Quick overview
- Entry: `src/main.tsx` (Bootstrap and Toast CSS are imported here)
- API agent: `src/api/agent.ts` (uses Axios)
- Config: `src/config.ts` (reads `VITE_API_BASE_URL`)
- Main UI pieces: `src/features/todos/*` (TodoForm, TodoTable, hooks)

Setup
1. Install dependencies:

```bash
cd frontend
npm install
```

2. Configure API base URL:
- Copy `.env.dev` to `.env` (or create `.env.local`) and set `VITE_API_BASE_URL`.
  - Example: `VITE_API_BASE_URL=https://localhost:7150/api`
- The app reads this value from `import.meta.env.VITE_API_BASE_URL` via `src/config.ts`.

3. Start the dev server:

```bash
npm run dev
```

Available npm scripts
- `npm run dev` - start development server (Vite)
- `npm run build` - build the production bundle
- `npm run preview` - preview the production build locally
- `npm run lint` - run ESLint