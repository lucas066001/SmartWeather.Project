"use client";

import { login } from "@/actions/auth";
import LoadingCircle from "@/components/icons/loadingCircle";
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from "@/components/ui/label";
import { useActionState } from "react";

const initialState = {};
function LoginPage() {
  const [state, formAction, pending] = useActionState(login, initialState);
  return (
    <>
      <h3 className="text-primary font-bold text-xl">Login</h3>
      <form className="flex flex-col gap-y-5 w-3/4" action={formAction}>
        <div className="flex flex-col gap-y-2">
          <Label className="text-secondary" htmlFor="email">Email adress</Label>
          <Input id="email" name="email" type="email" placeholder="Email" />
        </div>
        <div className="flex flex-col gap-y-2">
          <Label className="text-secondary" htmlFor="password">Password</Label>
          <Input
            id="password"
            name="password"
            type="password"
            placeholder="password"
          />
        </div>
        <div className="flex flex-col gap-2">
          <Button className="bg-primary hover:bg-lightPrimary font-bold" type="submit" variant={"default"}>
            {pending && <LoadingCircle color="#FFFFFF"></LoadingCircle>}Log in
          </Button>
          <a href="/register" className="text-secondary ml-auto text-xs">Register</a>
        </div>
      </form>
    </>
  );
}

export default LoginPage;
