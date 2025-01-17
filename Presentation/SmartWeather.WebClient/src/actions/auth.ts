"use server";

import { deleteToken, saveToken } from "@/lib/tokenManager";
import { register, signin } from "@/services/auth/auth_service";
import { Status } from "@/services/dtos/api";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import * as z from "zod";

const loginSchema = z.object({
  email: z.string().email({
    message: "Not valid",
  }),
  password: z.string().min(1, {
    message: "Required",
  }),
});

const registerSchema = z.object({
  name: z.string().min(1, {
    message: "Required",
  }),
  email: z.string().email({
    message: "Not valid",
  }),
  password: z.string().min(1, {
    message: "Required",
  }),
  confirmPassword : z.string(),
}).refine(data => data.password === data.confirmPassword, {
  message: "Passwords must match",
  path: ["confirmPassword"],
});

export async function login(_prevState: any, formData: FormData) {
  const parse = loginSchema.safeParse({
    email: formData.get("email"),
    password: formData.get("password"),
  });

  if(!parse.success) {
    return parse.error.flatten().fieldErrors;
  }
  const res = await signin(parse.data.email, parse.data.password);

  if (res.status != Status.OK) return { message: res.message };
  if(res.data) await saveToken(res.data?.token);
  revalidatePath("/dashboard");
  redirect("/dashboard");
}

export async function registerCheck(_prevState: any, formData: FormData) {
  console.log("enter registerCheck");
  const parse = registerSchema.safeParse({
    name: formData.get("name"),
    email: formData.get("email"),
    password: formData.get("password"),
    confirmPassword: formData.get("confirmPassword"),
  });

  if(!parse.success) {
    return parse.error.flatten().fieldErrors;
  }
  const res = await register(parse.data.name, parse.data.email, parse.data.password);

  if (res.status != Status.OK) return { message: res.message };
  if(res.data) await saveToken(res.data?.token);
  redirect("/dashboard");
}

export async function logout() {
  deleteToken();
  redirect("/");
}
