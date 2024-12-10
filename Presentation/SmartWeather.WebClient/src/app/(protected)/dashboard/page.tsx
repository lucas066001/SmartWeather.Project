import MainLayout from "@/components/ui/MainLayout";
import SocketConnector from "@/components/ui/socketConnector";

function DashboardPage() {
  return (
    <MainLayout title="Dashboard">
      <p className="font-outfit">Initialisation projet</p>
      <SocketConnector />
    </MainLayout>
  );
}

export default DashboardPage