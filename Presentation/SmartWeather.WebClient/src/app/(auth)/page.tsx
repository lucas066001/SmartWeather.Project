"use client"
import { login } from "@/actions/user";
import { useActionState } from "react";
import { useFormState } from 'react-dom'

const initialState: any = {

}
function DashboardPage() {
  const [state, formAction] = useActionState(login, initialState)

  return (
    <>
      <h3 className="text-primary font-bold text-xl">Login</h3>
      <form className="flex flex-col gap-3" action={formAction}>
        <div className="flex flex-col">
          <label htmlFor="email">Email adress</label>
          <input id="email" name="email" type="email" placeholder="email" />
        </div>
        <div className="flex flex-col">
          <label htmlFor="password">Password</label>
          <input id="password" name="password" type="password" placeholder="password" />
        </div>
        <button type="submit">Log in</button>
      </form>
    </>
  );
}

export default DashboardPage