"use server";

import { cookies } from "next/headers";

export async function saveToken(token: string) {
  const cookieStore = await cookies();

  cookieStore.set("authToken", token, {
    httpOnly: true,
    secure: true,
    sameSite: "strict",
    maxAge: 60 * 60 * 24,
    path: "/",
  });
}

export async function deleteToken() {
  const cookieStore = await cookies();

  cookieStore.delete("authToken");
}

export async function getToken() {
  const cookieStore = await cookies();

  return cookieStore.get("authToken");
}

export async function verifyToken() {
  const cookieStore = await cookies();
  return cookieStore.has("authToken");
}
