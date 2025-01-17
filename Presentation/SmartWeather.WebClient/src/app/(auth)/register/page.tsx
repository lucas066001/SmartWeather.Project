"use client";

import { registerCheck } from "@/actions/auth";
import LoadingCircle from "@/components/icons/loadingCircle";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { toast } from "@/hooks/use-toast";
import { useActionState, useEffect } from "react";

const initialState : any = {};

function RegisterPage() {
  const [state, formAction, pending] = useActionState(registerCheck, initialState);

  const getFieldError = (fieldName: string) => state.fieldErrors ? state.fieldErrors[fieldName]?.[0] || null : null;
  useEffect(() => {
    if(state.errorMessage){
      toast({
        className : "bg-alert text-white",
        title: "Uh oh! Something went wrong.",
        description: "There was a problem with your request.",
      })
    }
  },[state]);

  return (
    <>
      <h3 className="text-primary font-bold text-xl mx-auto">Register</h3>
      <form className="flex flex-col gap-y-5 w-3/4" action={formAction}>
        <div className="flex flex-col gap-y-2">
          <div className="flex flex-row">
            <Label className="text-secondary" htmlFor="name">Name</Label>
            {getFieldError("name") && (
              <span className="text-alert text-xs ml-auto">{getFieldError("name")}</span>
            )}
          </div>
          <Input id="name" name="name" type="name" placeholder="Name" />
        </div>
        <div className="flex flex-col gap-y-2">
          <div className="flex flex-row">
            <Label className="text-secondary" htmlFor="email">Email adress</Label>
            {getFieldError("email") && (
              <span className="text-alert text-xs ml-auto">{getFieldError("email")}</span>
            )}
          </div>
          <Input id="email" name="email" type="email" placeholder="Email" />
        </div>
        <div className="flex flex-col gap-y-2">
          <div className="flex flex-row">
            <Label className="text-secondary" htmlFor="password">Password</Label>
            {getFieldError("password") && (
              <span className="text-alert text-xs ml-auto">{getFieldError("password")}</span>
            )}
          </div>
          <Input
            id="password"
            name="password"
            type="password"
            placeholder="Password"
          />
        </div>
        <div className="flex flex-col gap-y-2">
          <div className="flex flex-row">
            <Label className="text-secondary" htmlFor="confirmPassword">Confirm Password</Label>
            {getFieldError("confirmPassword") && (
              <span className="text-alert text-xs ml-auto">{getFieldError("confirmPassword")}</span>
            )}
          </div>
          <Input
            id="confirmPassword"
            name="confirmPassword"
            type="password"
            placeholder="Confirm Password"
          />
        </div>
        <div className="flex flex-col gap-2">
          <Button className="bg-primary hover:bg-lightPrimary font-bold" type="submit" variant={"default"}>
          {pending && <LoadingCircle color="#FFFFFF"></LoadingCircle>} Register
          </Button>
          <a href="/" className="text-secondary ml-auto text-xs">Login</a>
        </div>
      </form>
    </>
  );
}

export default RegisterPage;
