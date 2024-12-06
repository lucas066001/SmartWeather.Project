import { NextRequest, NextResponse } from "next/server";
import { verifyToken } from "./lib/tokenManager";

export async function middleware(request: NextRequest) {
  const validToken = await verifyToken();

  if (
    (validToken && request.nextUrl.pathname === "/") ||
    (validToken && request.nextUrl.pathname === "/register")
  ) {
    return NextResponse.redirect(new URL("/dashboard", request.url));
  } 

  if (!validToken && request.nextUrl.pathname === "/dashboard") {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/dashboard", "/", "/register"],
};
