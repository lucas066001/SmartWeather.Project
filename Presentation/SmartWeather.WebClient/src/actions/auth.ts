"use server";

import { deleteToken, saveToken } from "@/lib/tokenManager";
import { signin } from "@/services/auth/auth_service";
import { Status } from "@/services/dtos/api";
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

export async function login(_prevState: any, formData: FormData) {
  const parse = loginSchema.safeParse({
    email: formData.get("email"),
    password: formData.get("password"),
  });

  if (parse.success) {
    const res = await signin(parse.data.email, parse.data.password);

    if (res.status != Status.OK) return { message: res.message };

    if (res.data) {
      saveToken(res.data?.token);

      redirect("/dashboard");
    }
  } else {
    // Gestion erreur specific
  }
  return {};
}

export async function logout() {
  deleteToken();
  redirect("/");
}
